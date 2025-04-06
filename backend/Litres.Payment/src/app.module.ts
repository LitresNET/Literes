import { Module } from '@nestjs/common';
import { TypeOrmModule } from '@nestjs/typeorm';
import { PaymentController } from './Controllers/PaymentController';
import { PaymentService } from './Services/PaymentService';
import { Payment } from './Models/Payment';
import { Transaction } from './Models/Transaction';
import { ConfigModule, ConfigService } from '@nestjs/config';
import { ClientsModule, Transport } from '@nestjs/microservices';

@Module({
  imports: [
    ConfigModule.forRoot(), // Загрузка переменных из .env
    TypeOrmModule.forRootAsync({
      imports: [ConfigModule],
      useFactory: () => ({
        type: 'postgres',
        host: 'litres_payment_db', // configService.get<string>('PAYMENT_DB_HOST'),
        port: 5432, // configService.get<number>('PAYMENT_DB_PORT'),
        username: 'postgres', // configService.get<string>('PAYMENT_DB_USER'),
        password: 'password', // configService.get<string>('PAYMENT_DB_PASSWORD'),
        database: 'payment_db', // configService.get<string>('PAYMENT_DB'),
        entities: [Payment, Transaction],
        synchronize: true,
      }),
      inject: [ConfigService],
    }),
    ClientsModule.register([
      {
        name: 'PAYMENT_PACKAGE',
        transport: Transport.GRPC,
        options: {
          package: 'payment',
          protoPath: join(__dirname, "./Protos/payment.proto"),
        },
      },
    ]),
  ],
  controllers: [PaymentController],
  providers: [PaymentService],
})
export class AppModule {}
