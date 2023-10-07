import { Component, OnInit } from '@angular/core';
import { Product } from '../shared/models/product';
import { ShopService } from './shop.service';
import { Brand } from '../shared/models/brand';
import { Type } from '../shared/models/type';
import { ShopParams } from '../shared/models/shopParams';

@Component({
  selector: 'app-shop',
  templateUrl: './shop.component.html',
  styleUrls: ['./shop.component.scss']
})
export class ShopComponent implements OnInit {
  products: Product[] = [];
  types: Type[] = [];
  brands: Brand[] = [];
  shopParams = new ShopParams();
  totalProductsCount = 0;
  sortOptions = [
    { name: 'Alphabetical', value: 'name' },
    { name: 'Price: Low to High', value: 'priceAsc' },
    { name: 'Price: High to Low', value: 'priceDsc' }
  ];

  constructor(private shopService: ShopService) {}

  ngOnInit(): void {
    this.getProducts();
    this.getTypes();
    this.getBrands();
  }

  getProducts() {
    this.shopService.getProducts(this.shopParams).subscribe(response => {
      this.products = response.data;
      this.shopParams.pageNumber = response.pageIndex;
      this.shopParams.pageSize = response.pageSize;
      this.totalProductsCount = response.count;
    });
  }

  getBrands() {
    this.shopService.getBrands().subscribe(response => {
      let all: Brand = { id: 0, name: 'All' };
      this.brands = [all, ...response];
    });
  }

  getTypes() {
    this.shopService.getTypes().subscribe(response => {
      let all: Type = { id: 0, name: 'All' };
      this.types = [all, ...response];
    });
  }

  onBrandSelected(brandId: number) {
    this.shopParams.brandId = brandId;
    this.shopParams.pageNumber = 1;
    this.getProducts();
  }

  onTypeSelected(typeId: number) {
    this.shopParams.typeId = typeId;
    this.shopParams.pageNumber = 1;
    this.getProducts();
  }

  onSortSelected(sort: string) {
    this.shopParams.sort = sort;
    this.getProducts();
  }

  onPageChange(event: any) {
    this.shopParams.pageNumber = event;
    this.getProducts();
  }

  onSearch() {
    this.shopParams.pageNumber = 1;
    this.getProducts();
  }

  onReset() {
    this.shopParams = new ShopParams();
    this.getProducts();
  }
}
