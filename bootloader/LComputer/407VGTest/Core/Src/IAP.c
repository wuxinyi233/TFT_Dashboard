#include "IAP.h"
#include "internalflash.h"
#include "can.h"
static uint32_t ulBuf_Flash_App [ 1024 ];
int IAP_Write_App_Bin ( uint32_t FlagStartAddr,uint32_t ulStartAddr, uint8_t * pBin_DataBuf, uint32_t ulBufLength )
{
	uint32_t t;
	uint16_t i=0;
	uint32_t temp;
	//uint32_t fwaddr=ulStartAddr;//当前写入地址
	uint8_t *dfu=pBin_DataBuf;
	for(t=0;t<ulBufLength;t+=4)
	{						   
		temp= (uint32_t)dfu[3]<<24;   
		temp|=(uint32_t)dfu[2]<<16;    
		temp|=(uint32_t)dfu[1]<<8;
		temp|=(uint32_t)dfu[0];	  
		dfu+=4;//偏移四个地址
		ulBuf_Flash_App[i++]=temp;	    
	} 
//flash数据写入必须是4字节对齐
	if(STMFLASH_Write(FlagStartAddr,ulStartAddr,ulBuf_Flash_App,i)!=0)//将最后的一些内容字节写进去
	{
		return -1;
	}else
	{
		return 0;
	}
	
}

__asm void MSR_MSP ( uint32_t ulAddr ) 
{
    MSR MSP, r0 			                   //set Main Stack value
    BX r14
}




//跳转到应用程序段
//ulAddr_App:用户代码起始地址.
void IAP_ExecuteApp ( uint32_t ulAddr_App )
{
	pIapFun_TypeDef pJump2App; 
	uint8_t i;
	
	if ( ( ( * ( vu32 * ) ulAddr_App ) & 0x2FFE0000 ) == 0x20000000 )	  //检查栈顶地址是否合法.（2fffffff-2ffe0000+1）/1024=128k
	{ 
		//关闭所有的中断
		HAL_CAN_MspDeInit(&hcan2);
		HAL_RCC_DeInit();	
		__disable_irq();
		__HAL_RCC_GPIOA_CLK_DISABLE();
		__HAL_RCC_GPIOB_CLK_DISABLE();
		__HAL_RCC_GPIOC_CLK_DISABLE();
		__HAL_RCC_GPIOH_CLK_DISABLE();
		//__set_BASEPRI(0x20);		//屏蔽到TIM2/3/4中断响应
		__set_PRIMASK(1);
		//__set_FAULTMASK(1);
		
	  //清除所有中断挂起标志位	
		for (i = 0; i < 8; i++)		
		{
			NVIC->ICER[i]=0xFFFFFFFF;
			NVIC->ICPR[i]=0xFFFFFFFF;
		}	
		//关闭掉系统滴答定时器，该定时器会在APP的程序中重新启动，调用HAL_Init();函数会启动；		
		SysTick->CTRL = 0;	
		SysTick->LOAD = 0;
		SysTick->VAL = 0;		
		//重新启动中断开关
		//__set_BASEPRI(0);		
		__enable_irq();
		__set_PRIMASK(0);
		//__set_FAULTMASK(0);
		
		
		pJump2App = ( pIapFun_TypeDef ) * ( vu32 * ) ( ulAddr_App + 4 );	//用户代码区第二个字为程序开始地址(复位地址)		
		MSR_MSP ( * ( vu32 * ) ulAddr_App );					                            //初始化APP堆栈指针(用户代码区的第一个字用于存放栈顶地址)
		pJump2App ();								                                    	//跳转到APP.
	}
	
}	
