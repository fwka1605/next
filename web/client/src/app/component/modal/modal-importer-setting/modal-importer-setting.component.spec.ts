import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ModalImporterSettingComponent } from './modal-importer-setting.component';

describe('ModalImporterSettingComponent', () => {
  let component: ModalImporterSettingComponent;
  let fixture: ComponentFixture<ModalImporterSettingComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ModalImporterSettingComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ModalImporterSettingComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
