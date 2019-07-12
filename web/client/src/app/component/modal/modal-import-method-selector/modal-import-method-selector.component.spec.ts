import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ModalImportMethodSelectorComponent } from './modal-import-method-selector.component';

describe('ModalImportMethodSelectorComponent', () => {
  let component: ModalImportMethodSelectorComponent;
  let fixture: ComponentFixture<ModalImportMethodSelectorComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ModalImportMethodSelectorComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ModalImportMethodSelectorComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
