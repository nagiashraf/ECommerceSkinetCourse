import { Component, Input } from '@angular/core';
import { faCartShopping } from '@fortawesome/free-solid-svg-icons';
import { Product } from 'src/app/shared/models/product';

@Component({
  selector: 'app-product-item',
  templateUrl: './product-item.component.html',
  styleUrls: ['./product-item.component.scss']
})
export class ProductItemComponent {
  @Input() product: Product | undefined;
  faCartShopping = faCartShopping;
}
