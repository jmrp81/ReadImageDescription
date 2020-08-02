import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { GaleryComponent } from './components/galery/galery.component';


const routes: Routes = [
  {
    path: '',
    component: GaleryComponent,
    data: {
    }
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes, { useHash: true })],
  exports: [RouterModule],
  providers: []
})
export class AppRoutingModule { }

