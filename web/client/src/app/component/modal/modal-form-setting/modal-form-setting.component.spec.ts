import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ModalFormSettingComponent } from './modal-form-setting.component';

describe('ModalFormSettingComponent', () => {
  let component: ModalFormSettingComponent;
  let fixture: ComponentFixture<ModalFormSettingComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ModalFormSettingComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ModalFormSettingComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
