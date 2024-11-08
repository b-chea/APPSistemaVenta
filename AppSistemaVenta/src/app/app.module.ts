import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { provideAnimationsAsync } from '@angular/platform-browser/animations/async';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { LoginComponent } from './Components/login/login.component';  
import { LayoutComponent } from './Components/layout/layout.component';

import { SharedModule } from './Reutilizable/shared/shared.module';
import { ModalUsuarioComponent } from './Components/layout/Modales/modal-usuario/modal-usuario.component';
import { ModalProductoComponent } from './Components/layout/Modales/modal-producto/modal-producto.component';


@NgModule({
  declarations: [
    AppComponent,
    LayoutComponent,
    LoginComponent,
    ModalUsuarioComponent,
    ModalProductoComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    SharedModule,
    BrowserAnimationsModule
  ],
  providers: [
    provideAnimationsAsync()
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
