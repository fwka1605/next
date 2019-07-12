import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { Pb1401GeneralSettingMasterComponent } from './pb1401-general-setting-master.component';

describe('Pb1401GeneralSettingMasterComponent', () => {
  let component: Pb1401GeneralSettingMasterComponent;
  let fixture: ComponentFixture<Pb1401GeneralSettingMasterComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ Pb1401GeneralSettingMasterComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(Pb1401GeneralSettingMasterComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
