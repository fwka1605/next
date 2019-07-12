import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { Pd0201ReceiptImporterComponent } from './pd0201-receipt-importer.component';

describe('Pd0201ReceiptImporterComponent', () => {
  let component: Pd0201ReceiptImporterComponent;
  let fixture: ComponentFixture<Pd0201ReceiptImporterComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ Pd0201ReceiptImporterComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(Pd0201ReceiptImporterComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
