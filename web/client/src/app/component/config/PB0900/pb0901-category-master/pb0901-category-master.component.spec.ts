import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { Pb0901CategoryMasterComponent } from './pb0901-category-master.component';

describe('Pb0901CategoryMasterComponent', () => {
  let component: Pb0901CategoryMasterComponent;
  let fixture: ComponentFixture<Pb0901CategoryMasterComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ Pb0901CategoryMasterComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(Pb0901CategoryMasterComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
