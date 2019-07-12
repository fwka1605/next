import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { Pd0101EbFileImporterComponent } from './pd0101-eb-file-importer.component';

describe('Pd0101EbFileImporterComponent', () => {
  let component: Pd0101EbFileImporterComponent;
  let fixture: ComponentFixture<Pd0101EbFileImporterComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ Pd0101EbFileImporterComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(Pd0101EbFileImporterComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
