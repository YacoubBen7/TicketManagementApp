import { NgModule } from '@angular/core';
import { PrimeNgModule } from './modules/prime-ng/prime-ng.module';
import { DateFormatPipe } from './pipes/date-format.pipe';
import { ModalComponent } from './components/modal/modal.component';



const MODULES = [PrimeNgModule];
const COMPONENTS = [DateFormatPipe, ModalComponent];
@NgModule({
  declarations: [...COMPONENTS],
  imports: [...MODULES],
  exports: [...MODULES, ...COMPONENTS],
})
export class SharedModule {}
