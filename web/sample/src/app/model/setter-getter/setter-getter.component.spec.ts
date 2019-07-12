import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { SetterGetterComponent } from './setter-getter.component';

describe('SetterGetterComponent', () => {
  let component: SetterGetterComponent;
  let fixture: ComponentFixture<SetterGetterComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ SetterGetterComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SetterGetterComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
