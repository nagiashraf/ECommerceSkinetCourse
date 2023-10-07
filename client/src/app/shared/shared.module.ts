import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { NgbPaginationModule } from '@ng-bootstrap/ng-bootstrap';
import { FormsModule } from '@angular/forms';



@NgModule({
  declarations: [],
  imports: [
    CommonModule,
    FormsModule,
    FontAwesomeModule,
    NgbPaginationModule
  ],
  exports: [
    FontAwesomeModule,
    FormsModule,
    NgbPaginationModule
  ]
})
export class SharedModule { }
