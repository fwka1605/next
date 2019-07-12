import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { PartsCompanyComponent } from './parts-company.component';

describe('PartsCompanyComponent', () => {
  let component: PartsCompanyComponent;
  let fixture: ComponentFixture<PartsCompanyComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ PartsCompanyComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(PartsCompanyComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
