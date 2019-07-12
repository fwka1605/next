import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { Pb0101CompanyMasterComponent } from './pb0101-company-master.component';

describe('Pb0101CompanyMasterComponent', () => {
  let component: Pb0101CompanyMasterComponent;
  let fixture: ComponentFixture<Pb0101CompanyMasterComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ Pb0101CompanyMasterComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(Pb0101CompanyMasterComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
