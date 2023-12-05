import { ComponentFixture, TestBed } from '@angular/core/testing';

import { NavbarItemListComponent } from './navbar-item-list.component';

describe('NavbarItemListComponent', () => {
  let component: NavbarItemListComponent;
  let fixture: ComponentFixture<NavbarItemListComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [NavbarItemListComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(NavbarItemListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
