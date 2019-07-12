import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ModalEbExcludeAccountingSettingComponent } from './modal-eb-exclude-accounting-setting.component';

describe('ModalEbExcludeAccountingSettingComponent', () => {
  let component: ModalEbExcludeAccountingSettingComponent;
  let fixture: ComponentFixture<ModalEbExcludeAccountingSettingComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ModalEbExcludeAccountingSettingComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ModalEbExcludeAccountingSettingComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
