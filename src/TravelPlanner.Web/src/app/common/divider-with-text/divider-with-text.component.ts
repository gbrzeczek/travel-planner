import { Component, Input } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatDividerModule } from '@angular/material/divider';

@Component({
  selector: 'app-divider-with-text',
  standalone: true,
  imports: [CommonModule, MatDividerModule],
  templateUrl: './divider-with-text.component.html',
  styleUrl: './divider-with-text.component.scss',
})
export class DividerWithTextComponent {}
