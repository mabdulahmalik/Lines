import { Component } from 'vue';
export interface TabType {
  label?: string;
  sub_label?: string;
  icon?: string | Component;
  badge?: '' | string | number | null;
  disabled?: boolean;
}
