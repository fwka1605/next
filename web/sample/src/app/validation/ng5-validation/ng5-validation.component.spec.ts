import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { Ng5ValidationComponent } from './ng5-validation.component';

describe('Ng5ValidationComponent', () => {
  let component: Ng5ValidationComponent;
  let fixture: ComponentFixture<Ng5ValidationComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ Ng5ValidationComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(Ng5ValidationComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
