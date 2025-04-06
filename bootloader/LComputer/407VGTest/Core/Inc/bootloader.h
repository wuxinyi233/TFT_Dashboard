#ifndef __BOOTLOADER_H
#define __BOOTLOADER_H

#include "main.h"
#define        CANRecv_FLASH_LEN  			              6320u       //定义 APP 固件最大容量，55kB=55*1024=56320
extern uint8_t RecAllState;
extern struct CAN_Upcom_RECIEVE                                              //串口数据帧的处理结构体
{
	uint8_t  ucDataBuf [ CANRecv_FLASH_LEN ];
	uint16_t  usLength;
	
	uint16_t totalPackCount;//总包数
	uint8_t lastPackContentCount;//最后一个包 帧长度
	uint32_t totalContentCount;//总数据长度
	uint8_t lastContentFrameLength;//最后一帧数据长度
	uint8_t packIndex;//0x00, 包编号 0
	uint16_t upPackCheckSum;//上位机传过来的累加和
	uint16_t packCheckSum;//累加和
	uint8_t contentIndex;//内容编号 1,2,3,..0xfe
	uint8_t recState;//接收状态1表示成功，0表示失败
} CanAppbin;
void Can_ReceivedHandle(uint8_t *rec_dat);
void Write_strAppBin(uint8_t *rec_dat,uint8_t len);
#endif /* __CAN_H__ */
