import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CategoryExcludeComponent } from './category-exclude.component';

describe('CategoryExcludeComponent', () => {
  let component: CategoryExcludeComponent;
  let fixture: ComponentFixture<CategoryExcludeComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CategoryExcludeComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CategoryExcludeComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
