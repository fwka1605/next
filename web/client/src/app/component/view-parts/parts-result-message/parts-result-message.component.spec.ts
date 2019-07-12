import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { PartsResultMessageComponent } from './parts-result-message.component';

describe('PartsResultMessageComponent', () => {
  let component: PartsResultMessageComponent;
  let fixture: ComponentFixture<PartsResultMessageComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ PartsResultMessageComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(PartsResultMessageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
