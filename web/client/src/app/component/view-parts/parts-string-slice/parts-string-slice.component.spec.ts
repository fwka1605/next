import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { PartsStringSliceComponent } from './parts-string-slice.component';

describe('PartsStringSliceComponent', () => {
  let component: PartsStringSliceComponent;
  let fixture: ComponentFixture<PartsStringSliceComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ PartsStringSliceComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(PartsStringSliceComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
