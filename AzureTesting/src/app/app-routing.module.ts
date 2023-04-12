import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { UpdateComponent } from './update/update.component';
import { DisplayComponent } from './display/display.component';
import { LoginComponent } from './login/login.component';


const routes: Routes = [
  //{ path: '', redirectTo: '/display', pathMatch: 'full' },
  { path: 'update', component: UpdateComponent },
  { path: 'display', component: DisplayComponent },
  {path:'login',component:LoginComponent}
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
