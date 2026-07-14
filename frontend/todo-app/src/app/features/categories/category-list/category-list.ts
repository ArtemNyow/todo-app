import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { RouterLink } from '@angular/router';
import { CategoryService } from '../../../core/services/category.service';
import { Category } from '../../../core/models/category.model';

@Component({
  selector: 'app-category-list',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterLink],
  templateUrl: './category-list.html',
  styleUrl: './category-list.scss',
})
export class CategoryListComponent implements OnInit {
  categories: Category[] = [];
  newCategoryName = '';
  editingId: number | null = null;
  editingName = '';
  errorMessage = '';
  isLoading = false;

  constructor(private categoryService: CategoryService) {}

  ngOnInit(): void {
    this.loadCategories();
  }

  loadCategories(): void {
    this.isLoading = true;
    this.categoryService.getCategories().subscribe({
      next: (categories) => {
        this.categories = categories;
        this.isLoading = false;
      },
      error: (err) => {
        this.errorMessage = 'Не вдалось завантажити категорії';
        this.isLoading = false;
        console.error(err);
      },
    });
  }

  onAdd(): void {
    if (!this.newCategoryName.trim()) return;

    this.categoryService.createCategory({ name: this.newCategoryName }).subscribe({
      next: () => {
        this.newCategoryName = '';
        this.loadCategories();
      },
      error: (err) => {
        this.errorMessage = 'Не вдалось додати категорію';
        console.error(err);
      },
    });
  }

  startEdit(category: Category): void {
    this.editingId = category.id;
    this.editingName = category.name;
  }

  cancelEdit(): void {
    this.editingId = null;
    this.editingName = '';
  }

  saveEdit(id: number): void {
    if (!this.editingName.trim()) return;

    this.categoryService.updateCategory(id, { name: this.editingName }).subscribe({
      next: () => {
        this.editingId = null;
        this.loadCategories();
      },
      error: (err) => {
        this.errorMessage = 'Не вдалось оновити категорію';
        console.error(err);
      },
    });
  }

  onDelete(id: number): void {
    if (!confirm('Видалити цю категорію? Задачі залишаться, але без категорії.')) return;

    this.categoryService.deleteCategory(id).subscribe({
      next: () => this.loadCategories(),
      error: (err) => {
        this.errorMessage = 'Не вдалось видалити категорію';
        console.error(err);
      },
    });
  }
}
