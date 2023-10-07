import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { Pagination } from '../shared/models/pagination';
import { Brand } from '../shared/models/brand';
import { Type } from '../shared/models/type';
import { ShopParams } from '../shared/models/shopParams';

@Injectable({
  providedIn: 'root'
})
export class ShopService {
  baseUrl = environment.apiUrl;

  constructor(private http: HttpClient) { }

  getProducts(shopParams: ShopParams) {
    let params = new HttpParams();

    if (shopParams.brandId > 0) {
      params = params.set('brandId', shopParams.brandId);
    }

    if (shopParams.typeId > 0) {
      params = params.set('typeId', shopParams.typeId);
    }

    if (shopParams.search) {
      params = params.set('search', shopParams.search);
    }

    params = params.set('sort', shopParams.sort);
    params = params.set('pageIndex', shopParams.pageNumber);
    params = params.set('pageSize', shopParams.pageSize);

    return this.http.get<Pagination>(this.baseUrl + 'products', { params });
  }

  getBrands() {
    return this.http.get<Brand[]>(this.baseUrl + 'products/brands');
  }

  getTypes() {
    return this.http.get<Type[]>(this.baseUrl + 'products/types');
  }
}
