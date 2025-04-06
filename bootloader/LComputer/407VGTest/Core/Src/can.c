/* USER CODE BEGIN Header */
/**
 ******************************************************************************
 * @file    can.c
 * @brief   This file provides code for the configuration
 *          of the CAN instances.
 ******************************************************************************
 * @attention
 *
 * Copyright (c) 2025 STMicroelectronics.
 * All rights reserved.
 *
 * This software is licensed under terms that can be found in the LICENSE file
 * in the root directory of this software component.
 * If no LICENSE file comes with this software, it is provided AS-IS.
 *
 ******************************************************************************
 */
/* USER CODE END Header */
/* Includes ------------------------------------------------------------------*/
#include "can.h"

/* USER CODE BEGIN 0 */
#include "bootloader.h"
uint8_t can_rec_dat[8];
/* USER CODE END 0 */

CAN_HandleTypeDef hcan2;

/* CAN2 init function */
void MX_CAN2_Init(void)
{

  /* USER CODE BEGIN CAN2_Init 0 */

  /* USER CODE END CAN2_Init 0 */

  /* USER CODE BEGIN CAN2_Init 1 */

  /* USER CODE END CAN2_Init 1 */
  hcan2.Instance = CAN2;
  hcan2.Init.Prescaler = 6;
  hcan2.Init.Mode = CAN_MODE_NORMAL;
  hcan2.Init.SyncJumpWidth = CAN_SJW_1TQ;
  hcan2.Init.TimeSeg1 = CAN_BS1_3TQ;
  hcan2.Init.TimeSeg2 = CAN_BS2_3TQ;
  hcan2.Init.TimeTriggeredMode = DISABLE;
  hcan2.Init.AutoBusOff = DISABLE;
  hcan2.Init.AutoWakeUp = DISABLE;
  hcan2.Init.AutoRetransmission = DISABLE;
  hcan2.Init.ReceiveFifoLocked = DISABLE;
  hcan2.Init.TransmitFifoPriority = DISABLE;
  if (HAL_CAN_Init(&hcan2) != HAL_OK)
  {
    Error_Handler();
  }
  /* USER CODE BEGIN CAN2_Init 2 */

  /* USER CODE END CAN2_Init 2 */

}

void HAL_CAN_MspInit(CAN_HandleTypeDef* canHandle)
{

  GPIO_InitTypeDef GPIO_InitStruct = {0};
  if(canHandle->Instance==CAN2)
  {
  /* USER CODE BEGIN CAN2_MspInit 0 */

  /* USER CODE END CAN2_MspInit 0 */
    /* CAN2 clock enable */
    __HAL_RCC_CAN2_CLK_ENABLE();
    __HAL_RCC_CAN1_CLK_ENABLE();

    __HAL_RCC_GPIOB_CLK_ENABLE();
    /**CAN2 GPIO Configuration
    PB12     ------> CAN2_RX
    PB13     ------> CAN2_TX
    */
    GPIO_InitStruct.Pin = GPIO_PIN_12|GPIO_PIN_13;
    GPIO_InitStruct.Mode = GPIO_MODE_AF_PP;
    GPIO_InitStruct.Pull = GPIO_PULLUP;
    GPIO_InitStruct.Speed = GPIO_SPEED_FREQ_VERY_HIGH;
    GPIO_InitStruct.Alternate = GPIO_AF9_CAN2;
    HAL_GPIO_Init(GPIOB, &GPIO_InitStruct);

    /* CAN2 interrupt Init */
    HAL_NVIC_SetPriority(CAN2_RX0_IRQn, 0, 0);
    HAL_NVIC_EnableIRQ(CAN2_RX0_IRQn);
  /* USER CODE BEGIN CAN2_MspInit 1 */

  /* USER CODE END CAN2_MspInit 1 */
  }
}

void HAL_CAN_MspDeInit(CAN_HandleTypeDef* canHandle)
{

  if(canHandle->Instance==CAN2)
  {
  /* USER CODE BEGIN CAN2_MspDeInit 0 */

  /* USER CODE END CAN2_MspDeInit 0 */
    /* Peripheral clock disable */
    __HAL_RCC_CAN2_CLK_DISABLE();
    __HAL_RCC_CAN1_CLK_DISABLE();

    /**CAN2 GPIO Configuration
    PB12     ------> CAN2_RX
    PB13     ------> CAN2_TX
    */
    HAL_GPIO_DeInit(GPIOB, GPIO_PIN_12|GPIO_PIN_13);

    /* CAN2 interrupt Deinit */
    HAL_NVIC_DisableIRQ(CAN2_RX0_IRQn);
  /* USER CODE BEGIN CAN2_MspDeInit 1 */

  /* USER CODE END CAN2_MspDeInit 1 */
  }
}

/* USER CODE BEGIN 1 */
void CAN2_Filter_Config(CAN_HandleTypeDef *hcan)
{
  CAN_FilterTypeDef sFilterConfig; // 定义 HAL 库过滤器配置结构体

  // 配置过滤器参数
  sFilterConfig.FilterBank = 14;                         // 过滤器编号0
  sFilterConfig.FilterMode = CAN_FILTERMODE_IDMASK;      // 使用标识符掩码模式
  sFilterConfig.FilterScale = CAN_FILTERSCALE_32BIT;     // 使用32位过滤器
	sFilterConfig.FilterIdHigh= ((((uint32_t)0x1314<<3)|
										 CAN_ID_EXT|CAN_RTR_DATA)&0xFFFF0000)>>16;		//ÒªÉ¸Ñ¡µÄID¸ßÎ» 
	sFilterConfig.FilterIdLow= (((uint32_t)0x1314<<3)|
									     CAN_ID_EXT|CAN_RTR_DATA)&0xFFFF; //ÒªÉ¸Ñ¡µÄIDµÍÎ» 
	sFilterConfig.FilterMaskIdHigh= 0xFFFF;			//É¸Ñ¡Æ÷¸ß16Î»Ã¿Î»±ØÐëÆ¥Åä
	sFilterConfig.FilterMaskIdLow= 0xFFFF;			//É¸Ñ¡Æ÷µÍ16Î»Ã¿Î»±ØÐëÆ¥Åä
  sFilterConfig.FilterFIFOAssignment = CAN_FILTER_FIFO0; // 过滤器关联到 FIFO0
  sFilterConfig.FilterActivation = ENABLE;               // 激活过滤器
  sFilterConfig.SlaveStartFilterBank  = 28; // 从过滤器起始编号（用于多CAN）或许不需要

  // 调用 HAL 库函数配置过滤器
  if (HAL_CAN_ConfigFilter(hcan, &sFilterConfig) != HAL_OK)
  {
    // 如果配置失败，则打印错误信息或执行错误处理
    // printf("CAN Filter configuration failed!\n");
    Error_Handler();
  }
  // 启动CAN外围设备
  if (HAL_CAN_Start(hcan) != HAL_OK)
  {
    Error_Handler();
  }

  // 激活可以RX通知
  if (HAL_CAN_ActivateNotification(hcan, CAN_IT_RX_FIFO0_MSG_PENDING) != HAL_OK)
  {
    Error_Handler();
  }
}
/**
 * @brief           CAN接收回调函数
 */
// can接收buffer,这里用全局变量保存，以便其他函数处理
void HAL_CAN_RxFifo0MsgPendingCallback(CAN_HandleTypeDef *hcan)
{
  CAN_RxHeaderTypeDef can_rx_header; // 接收

  if (hcan->Instance == CAN2)
  {
    HAL_CAN_GetRxMessage(hcan, CAN_FILTER_FIFO0, &can_rx_header, can_rec_dat);
		Can_ReceivedHandle(can_rec_dat);
  }
  	Tim_Count=0;//定时器计数清零
}
/**
 * @brief           CAN发送一组数据
 * @param[in]       msg：待发送的数据（最大为8个字节）
 * @param[in]       len：数据帧的长度（最大为8）
 * @param[in]       std_id：CAN_ID
 * @return          0：发送成功   1：发送失败
 */
uint8_t CAN_Send_Msg(uint8_t *msg, uint8_t len, uint8_t std_id)
{
  CAN_TxHeaderTypeDef can_tx_header;
  uint32_t TxMailbox;
  can_tx_header.StdId = std_id;
	can_tx_header.ExtId = std_id;
  can_tx_header.IDE = CAN_ID_EXT;
  can_tx_header.RTR = CAN_RTR_DATA;
  can_tx_header.TransmitGlobalTime = ENABLE;
  can_tx_header.DLC = len;
  // MailboxesFreeLevel = HAL_CAN_GetTxMailboxesFreeLevel(&hcan);
  while (HAL_CAN_GetTxMailboxesFreeLevel(&hcan2) == 0);
  if (HAL_CAN_AddTxMessage(&hcan2, &can_tx_header, msg, &TxMailbox) != HAL_OK) // 发送
  {
    return 1;
  }
  return 0;
}

/* USER CODE END 1 */
