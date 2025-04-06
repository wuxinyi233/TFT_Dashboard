#ifndef __IAP_H__
#define __IAP_H__

#include "main.h"
#include <stdint.h>
typedef uint32_t  u32;
typedef __IO uint32_t  vu32;
/************************** IAP 宏参数定义********************************/
#define                  macUser_Flash                                         //是否更新 APP 到 FLASH，否则更新到 RAM


#ifdef                   macUser_Flash
   #define               macAPP_START_ADDR		                     0x8010080  	//应用程序起始地址(存放在FLASH)
	 #define               FlashFlag_Addr                            0x8010000    //标志位起始地址
#else
   #define               macAPP_START_ADDR		                     0x20001000  	//应用程序起始地址(存放在RAM)
#endif
/************************** IAP 数据类型定义********************************/
typedef  void ( * pIapFun_TypeDef ) ( void );				                            //定义一个函数类型的参数.

/************************** IAP 外部变量********************************/
#define        macAPP_FLASH_LEN  			              56320u                     //定义 APP 固件最大容量，55kB=55*1024=56320

extern struct  STRUCT_IAP_RECIEVE                                              //串口数据帧的处理结构体
{
	uint8_t   ucDataBuf [ macAPP_FLASH_LEN ];
	uint16_t  usLength;
} strAppBin;



/************************** IAP 函数声明********************************/
int                  IAP_Write_App_Bin                       ( uint32_t flagaddr,uint32_t appxaddr, uint8_t * appbuf, uint32_t applen);
//int                  IAP_Write_App_Bin                       ( uint32_t appxaddr, uint8_t * appbuf, uint32_t applen);	//在指定地址开始,写入bin
void                  IAP_ExecuteApp                          ( uint32_t appxaddr );			                              //执行flash里面的app程序


//int IAP_Write_App_Bin ( uint32_t ulStartAddr, uint8_t * pBin_DataBuf, uint32_t ulBufLength );
int IAP_Write_App_Bin ( uint32_t FlagStartAddr,uint32_t ulStartAddr, uint8_t * pBin_DataBuf, uint32_t ulBufLength );
void IAP_ExecuteApp ( uint32_t ulAddr_App );

#endif
