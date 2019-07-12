import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ModalEbFileSettingComponent } from './modal-eb-file-setting.component';

describe('ModalEbFileSettingComponent', () => {
  let component: ModalEbFileSettingComponent;
  let fixture: ComponentFixture<ModalEbFileSettingComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ModalEbFileSettingComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ModalEbFileSettingComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
