import { Body, Controller, Get, Render } from '@nestjs/common';

@Controller('/')
export class ListController {
  @Get()
  @Render('productListPage')
  getProductListPage(@Body() goodList: ProductDto[]) {
    return { goodList };
  }
}
