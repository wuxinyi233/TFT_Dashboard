#include "internalflash.h"

#define STM32_FLASH_BASE 0x08000000
static uint32_t GetSector(uint32_t Address);
/**
  * @brief  flash写入
  * @param  None
  * @retval None
  */
	
int STMFLASH_Write(uint32_t FlagAddr,uint32_t WriteAddr, uint32_t *pBuffer, uint32_t NumToWrite)
{

	uint32_t startAddr = 0, EndAddr = 0;
	uint32_t FirstSector=0;
	uint32_t NbOfSectors=0;
	uint32_t Address = 0;
	uint32_t SECTORError = 0;
	int8_t state=0,N=0;
	uint32_t ReadSumData=0,WriteSumData=0;
	HAL_StatusTypeDef Flash_Status = HAL_ERROR;
	static FLASH_EraseInitTypeDef EraseInitStruct;
	if (WriteAddr < STM32_FLASH_BASE || WriteAddr % 4)
	return 0;	
	
	HAL_FLASH_Unlock();
			__HAL_FLASH_DATA_CACHE_DISABLE();
	__HAL_FLASH_CLEAR_FLAG(FLASH_FLAG_EOP    | FLASH_FLAG_OPERR | FLASH_FLAG_WRPERR | \
                         FLASH_FLAG_PGAERR | FLASH_FLAG_PGPERR | FLASH_FLAG_PGSERR);
	startAddr=WriteAddr;
	EndAddr=WriteAddr+NumToWrite*4;
	
	
	FirstSector = GetSector(startAddr);
	NbOfSectors = GetSector(EndAddr)- FirstSector + 1;
	

	/* Fill EraseInit structure*/
	EraseInitStruct.TypeErase     = FLASH_TYPEERASE_SECTORS;
	EraseInitStruct.VoltageRange  = FLASH_VOLTAGE_RANGE_3;
	EraseInitStruct.Sector        = FirstSector;
	EraseInitStruct.NbSectors     = NbOfSectors;
	
	Flash_Status = HAL_FLASHEx_Erase(&EraseInitStruct, &SECTORError);
	while(Flash_Status!=HAL_OK && N<3)
	{
		N++;
		FLASH_WaitForLastOperation(50000U);
		Flash_Status = HAL_FLASHEx_Erase(&EraseInitStruct, &SECTORError);
	}
	FLASH_WaitForLastOperation(50000U);
	if (Flash_Status!= HAL_OK)
	{

		state= -1;
		goto tolock;
	}	
	/*********************************/
	Address = startAddr;
	HAL_FLASH_Unlock();
	N = 0;
	while (Address < EndAddr)
	{
	//	if (FLASH_ProgramWord(startAddr, *pBuffer) != FLASH_COMPLETE) 
		if (HAL_FLASH_Program(FLASH_TYPEPROGRAM_WORD, Address,  *pBuffer) != HAL_OK)
		{
				N++;
		}
		else
		{
			N=0;
		  Address = Address + 4;
			WriteSumData+=*pBuffer;
			pBuffer++;
		}
		if(N>2)
		{ 
				 state= -1;
			    break;
		}
	}
	
	/*********************************/
	Address=WriteAddr;

	while (Address < EndAddr)
	{
		ReadSumData += *(__IO uint32_t*)Address;
		Address = Address + 4;
	}
	if(ReadSumData!=WriteSumData)
	{
		state= -1;
	}else
	{
		if (HAL_FLASH_Program(FLASH_TYPEPROGRAM_BYTE, FlagAddr,  0x55) != HAL_OK)
		{
			state= -1;
		}		
	}
	
	

	tolock:
	 __HAL_FLASH_DATA_CACHE_ENABLE();

	HAL_FLASH_Lock(); 
	if(state==-1)
	{
		return -1;
	}
	return 0;	
	
	
	
}



// 读取指定地址的字数据（32 位）
uint32_t ReadFlashWord(uint32_t address)
{
    return *(volatile uint32_t*)address;
}



uint32_t ReadArrayData(uint32_t *buffer, uint32_t length)
{
	uint32_t CheckSum=0;
    for (uint32_t i = 0; i < length; i++)
    {
        CheckSum+= * buffer;
    }
		return CheckSum;
}




/**
  * @brief  根据输入的地址给出它所在的sector
  *					例如：
						uwStartSector = GetSector(FLASH_USER_START_ADDR);
						uwEndSector = GetSector(FLASH_USER_END_ADDR);	
  * @param  Address：地址
  * @retval 地址所在的sector
  */
static uint32_t GetSector(uint32_t Address)
{
  uint32_t sector = 0;
  
  if((Address < ADDR_FLASH_SECTOR_1) && (Address >= ADDR_FLASH_SECTOR_0))
  {
    sector = FLASH_SECTOR_0;  
  }
  else if((Address < ADDR_FLASH_SECTOR_2) && (Address >= ADDR_FLASH_SECTOR_1))
  {
    sector = FLASH_SECTOR_1;  
  }
  else if((Address < ADDR_FLASH_SECTOR_3) && (Address >= ADDR_FLASH_SECTOR_2))
  {
    sector = FLASH_SECTOR_2;  
  }
  else if((Address < ADDR_FLASH_SECTOR_4) && (Address >= ADDR_FLASH_SECTOR_3))
  {
    sector = FLASH_SECTOR_3;  
  }
  else if((Address < ADDR_FLASH_SECTOR_5) && (Address >= ADDR_FLASH_SECTOR_4))
  {
    sector = FLASH_SECTOR_4;  
  }
  else if((Address < ADDR_FLASH_SECTOR_6) && (Address >= ADDR_FLASH_SECTOR_5))
  {
    sector = FLASH_SECTOR_5;  
  }
  else if((Address < ADDR_FLASH_SECTOR_7) && (Address >= ADDR_FLASH_SECTOR_6))
  {
    sector = FLASH_SECTOR_6;  
  }
  else/*(Address < FLASH_END_ADDR) && (Address >= ADDR_FLASH_SECTOR_23))*/
  {
    sector = FLASH_SECTOR_7;  
  }
  return sector;
}


