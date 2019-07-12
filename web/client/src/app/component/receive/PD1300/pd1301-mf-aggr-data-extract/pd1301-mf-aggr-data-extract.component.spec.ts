import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { Pd1301MfAggrDataExtractComponent } from './pd1301-mf-aggr-data-extract.component';

describe('Pd1301MfAggrDataExtractComponent', () => {
  let component: Pd1301MfAggrDataExtractComponent;
  let fixture: ComponentFixture<Pd1301MfAggrDataExtractComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ Pd1301MfAggrDataExtractComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(Pd1301MfAggrDataExtractComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
