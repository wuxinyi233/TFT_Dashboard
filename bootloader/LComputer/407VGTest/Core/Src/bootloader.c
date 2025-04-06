#include "bootloader.h"
struct CAN_Upcom_RECIEVE CanAppbin __attribute__((at(0x20002000))) = {{0}, 0};
uint8_t lastFrame = 0;
uint8_t lastFramLen=7;
uint8_t RecAllState=0;
void Can_Ack_Success()
{
	RecState = 1;//接收状态初始化
	SendFlag = 1;//发送状态初始化
}
void BootLoader_Init(uint32_t bytesCount)
{
	lastFrame = 0;
	lastFramLen=7;
	RecState = 0;//接收状态初始化
	SendFlag = 0;//发送状态初始化
	RecAllState=0;
	CanAppbin.contentIndex = 1;
	CanAppbin.packIndex = 0;
	CanAppbin.usLength = 0;//所有数据长度（实时变化）
	CanAppbin.recState = 0;//接收状态
	CanAppbin.totalContentCount = bytesCount;//总字节数据长度
	CanAppbin.totalPackCount = bytesCount/1778;
	if(bytesCount%1778>0)
	{
		CanAppbin.totalPackCount++;
	}
	CanAppbin.lastPackContentCount = bytesCount%1778/7;
	CanAppbin.lastContentFrameLength = bytesCount%1778%7;
	if(CanAppbin.lastContentFrameLength>0)
	{
		CanAppbin.lastPackContentCount++;
	}
}
void Can_Ack_Fail()
{
	RecState = 2;//接收状态初始化
	SendFlag = 1;//发送状态初始化
}
/// @brief can接收数据处理
/// @param rec_dat 8个字节数组
void Can_ReceivedHandle(uint8_t *rec_dat)
{
    if (rec_dat[0] == 0xff) // 总帧，不计算
    {
			BootLoader_Init((rec_dat[1]<<24)|(rec_dat[2]<<16)|(rec_dat[3]<<8)|rec_dat[4]);
			Can_Ack_Success();
			return;
    }
		if(CanAppbin.recState !=0)
		{
			goto EndTag;	
		}
    if (rec_dat[0] == 0x00)
    {
			//判断包序号
			if(CanAppbin.packIndex != (rec_dat[1]<<8|rec_dat[2]))
			{
				CanAppbin.recState = 2;//表示接收失败
				goto EndTag;
			}
			CanAppbin.contentIndex = 1;
			CanAppbin.packCheckSum = 0;
			
			CanAppbin.upPackCheckSum = rec_dat[5]<<8|rec_dat[6];
			Can_Ack_Success();
			return;
    }
    if (rec_dat[0] > 0x00&&rec_dat[0]<0xff)//数据帧
    {
			 //判断帧序号
			if(CanAppbin.contentIndex != rec_dat[0])
			{
				CanAppbin.recState = 2;//表示接收失败
				goto EndTag;
			}
			CanAppbin.contentIndex++;//当次就加一为下次作准备
			
			if(CanAppbin.packIndex+1 < CanAppbin.totalPackCount)
			{
				if(CanAppbin.contentIndex== 0xff)
				{
					lastFrame = 1;
				}
			}
			else 
			{
				if(CanAppbin.contentIndex-1 == CanAppbin.lastPackContentCount)
				{
					lastFrame = 1;
					if(CanAppbin.lastContentFrameLength>0)
					{
						lastFramLen = CanAppbin.lastContentFrameLength;
					}
					//表示完成
					RecAllState=1;
					
				}
			}
      // 数据帧
      Write_strAppBin((rec_dat + 1), lastFramLen);
			
			if(lastFrame == 1)
			{
				if(CanAppbin.upPackCheckSum != CanAppbin.packCheckSum)
				{
						CanAppbin.recState = 2;//表示接收失败
						goto EndTag;
				}
				//表示接收完成
				CanAppbin.packIndex++;
				lastFrame = 0;
			}
				if(RecAllState==1)
				{
					RecAllState=2;//所有数据发送完成
				}
				Can_Ack_Success();
    }
		EndTag:
		if(CanAppbin.recState ==2)
		{
			Can_Ack_Fail();
		}
}

/// @brief 写入数据到CanAppbin
/// @param rec_dat 字节数组
/// @param len 数组长度
void Write_strAppBin(uint8_t *rec_dat, uint8_t len)
{
    uint8_t i = 0;
    for (i = 0; i < len; i++)
    {
        CanAppbin.ucDataBuf[CanAppbin.usLength] = rec_dat[i];
        CanAppbin.usLength += 1;
				CanAppbin.packCheckSum += rec_dat[i];
    }
}
