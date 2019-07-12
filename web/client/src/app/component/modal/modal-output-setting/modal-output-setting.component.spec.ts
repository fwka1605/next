import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ModalOutputSettingComponent } from './modal-output-setting.component';

describe('ModalOutputSettingComponent', () => {
  let component: ModalOutputSettingComponent;
  let fixture: ComponentFixture<ModalOutputSettingComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ModalOutputSettingComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ModalOutputSettingComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
