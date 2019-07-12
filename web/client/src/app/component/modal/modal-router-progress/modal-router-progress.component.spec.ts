import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ModalRouterProgressComponent } from './modal-router-progress.component';

describe('ModalRouterProgressComponent', () => {
  let component: ModalRouterProgressComponent;
  let fixture: ComponentFixture<ModalRouterProgressComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ModalRouterProgressComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ModalRouterProgressComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
