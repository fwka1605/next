import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CategoryBillingComponent } from './category-billing.component';

describe('CategoryBillingComponent', () => {
  let component: CategoryBillingComponent;
  let fixture: ComponentFixture<CategoryBillingComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CategoryBillingComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CategoryBillingComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
