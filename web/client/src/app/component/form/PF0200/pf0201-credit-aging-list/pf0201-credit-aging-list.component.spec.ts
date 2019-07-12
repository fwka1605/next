import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { Pf0201CreditAgingListComponent } from './pf0201-credit-aging-list.component';

describe('Pf0201CreditAgingListComponent', () => {
  let component: Pf0201CreditAgingListComponent;
  let fixture: ComponentFixture<Pf0201CreditAgingListComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ Pf0201CreditAgingListComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(Pf0201CreditAgingListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
