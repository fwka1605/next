import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CategoryReceiptComponent } from './category-receipt.component';

describe('CategoryReceiptComponent', () => {
  let component: CategoryReceiptComponent;
  let fixture: ComponentFixture<CategoryReceiptComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CategoryReceiptComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CategoryReceiptComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
