import { PaymentService } from '../Services/PaymentService';
import { GetDataService } from 'src/Services/GetDataService';
import { Response } from 'express';
export declare class PaymentController {
    private readonly paymentService;
    private readonly getDataService;
    constructor(paymentService: PaymentService, getDataService: GetDataService);
    getPaymentPage(query: any): Promise<{
        totalPrice: number;
        orderId: number;
    }>;
    pay(body: any, res: Response): Promise<void>;
}
