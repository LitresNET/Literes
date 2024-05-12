"use strict";
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};
var __param = (this && this.__param) || function (paramIndex, decorator) {
    return function (target, key) { decorator(target, key, paramIndex); }
};
Object.defineProperty(exports, "__esModule", { value: true });
exports.PaymentController = void 0;
const common_1 = require("@nestjs/common");
const PaymentService_1 = require("../Services/PaymentService");
const GetDataService_1 = require("../Services/GetDataService");
let PaymentController = class PaymentController {
    constructor(paymentService, getDataService) {
        this.paymentService = paymentService;
        this.getDataService = getDataService;
    }
    async getPaymentPage(query) {
        let goods = await this.getDataService.getOrderData(query.orderId);
        let totalPrice = 0;
        let orderId = goods.orderId;
        goods.products.forEach(function (good) {
            totalPrice += good.price * good.amount;
        });
        return { totalPrice, orderId };
    }
    async pay(body, res) {
        const orderId = body.orderId;
        if (await this.paymentService.tryPay()) {
            return res.redirect(`http://localhost:5032/api/order/${orderId}/finish?isSuccess=true`);
        }
        else {
            return res.redirect(`http://localhost:5032/api/order/${orderId}/finish?isSuccess=false`);
        }
    }
};
exports.PaymentController = PaymentController;
__decorate([
    (0, common_1.Get)(),
    (0, common_1.Render)('paymentPage'),
    __param(0, (0, common_1.Query)()),
    __metadata("design:type", Function),
    __metadata("design:paramtypes", [Object]),
    __metadata("design:returntype", Promise)
], PaymentController.prototype, "getPaymentPage", null);
__decorate([
    (0, common_1.Post)(),
    __param(0, (0, common_1.Body)()),
    __param(1, (0, common_1.Res)()),
    __metadata("design:type", Function),
    __metadata("design:paramtypes", [Object, Object]),
    __metadata("design:returntype", Promise)
], PaymentController.prototype, "pay", null);
exports.PaymentController = PaymentController = __decorate([
    (0, common_1.Controller)('pay'),
    __metadata("design:paramtypes", [PaymentService_1.PaymentService, GetDataService_1.GetDataService])
], PaymentController);
//# sourceMappingURL=PaymentController.js.map