import { Component } from '@angular/core';
import { NavbarItemListComponent } from './navbar-item-list/navbar-item-list.component';

@Component({
  selector: 'tp-navbar',
  standalone: true,
  templateUrl: './navbar.component.html',
  styleUrl: './navbar.component.scss',
  imports: [NavbarItemListComponent],
})
export class NavbarComponent {}
