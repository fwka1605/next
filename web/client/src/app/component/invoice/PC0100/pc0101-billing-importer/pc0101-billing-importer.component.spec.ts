import { Pc0101BillingImporterComponent } from './pc0101-billing-importer.component';
import { async, ComponentFixture, TestBed } from '@angular/core/testing';


describe('Pc0101BillingImporterComponent', () => {
  let component: Pc0101BillingImporterComponent;
  let fixture: ComponentFixture<Pc0101BillingImporterComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [Pc0101BillingImporterComponent]
    })
      .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(Pc0101BillingImporterComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
