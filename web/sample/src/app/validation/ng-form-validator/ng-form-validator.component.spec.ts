import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { NgFormValidatorComponent } from './ng-form-validator.component';

describe('NgFormValidatorComponent', () => {
  let component: NgFormValidatorComponent;
  let fixture: ComponentFixture<NgFormValidatorComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ NgFormValidatorComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(NgFormValidatorComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
