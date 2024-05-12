import { Injectable } from '@nestjs/common';
import { HttpService } from '@nestjs/axios';
import axios from 'axios';

@Injectable()
export class GetDataService {
  async getOrderData(orderId:number) {
    const response = await axios({
        method: 'GET',
        url: `http://localhost:5032/api/order/${orderId}/info`,
    });

   return response.data;
  }
}
