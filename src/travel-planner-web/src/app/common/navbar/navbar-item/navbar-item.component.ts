import { Component, Input, OnChanges } from '@angular/core';
import { NavbarItem } from './navbar-item.model';

@Component({
  selector: 'tp-navbar-item',
  standalone: true,
  imports: [],
  templateUrl: './navbar-item.component.html',
  styleUrl: './navbar-item.component.scss',
})
export class NavbarItemComponent implements OnChanges {
  @Input({ required: true }) item!: NavbarItem;

  ngOnChanges() {
    console.log('NavbarItemComponent.ngOnChanges()');
  }
}
