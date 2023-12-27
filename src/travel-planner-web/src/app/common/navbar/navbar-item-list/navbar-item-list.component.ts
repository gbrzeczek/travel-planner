import { Component } from '@angular/core';
import { NavbarItemComponent } from '../navbar-item/navbar-item.component';
import { NavbarItem } from '../navbar-item/navbar-item.model';
import { RouterLink } from '@angular/router';

@Component({
  selector: 'tp-navbar-item-list',
  standalone: true,
  template: `
    @for (item of items; track $index) {
      <tp-navbar-item [item]="item" [routerLink]="item.path" />
    }
  `,
  styleUrl: './navbar-item-list.component.scss',
  imports: [NavbarItemComponent, RouterLink],
})
export class NavbarItemListComponent {
  items: NavbarItem[] = [
    { label: 'Home', path: '/', icon: 'bi bi-house-door' },
    { label: 'Trips', path: '/my-trips', icon: 'bi bi-suitcase-lg' },
    { label: 'Contact', path: '/contact', icon: 'bi bi-telephone' },
  ];
}
