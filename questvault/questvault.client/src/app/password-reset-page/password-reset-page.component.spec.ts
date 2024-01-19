import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PasswordResetPageComponent } from './password-reset-page.component';

describe('PasswordResetPageComponent', () => {
  let component: PasswordResetPageComponent;
  let fixture: ComponentFixture<PasswordResetPageComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [PasswordResetPageComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(PasswordResetPageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
