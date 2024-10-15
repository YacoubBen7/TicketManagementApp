import { NgModule } from '@angular/core';
import { ButtonModule } from 'primeng/button';
import { TableModule } from 'primeng/table';
import { PaginatorModule } from 'primeng/paginator';
import { DialogModule } from 'primeng/dialog';
import { InputTextModule } from 'primeng/inputtext';
import { DropdownModule } from 'primeng/dropdown';



const PrimeNgModules = [
  DialogModule,
  InputTextModule,
  ButtonModule,
  TableModule,
  PaginatorModule,
  DropdownModule,

];


@NgModule({
  declarations: [],
  imports: [...PrimeNgModules],
  exports: [...PrimeNgModules],
})
export class PrimeNgModule {}
