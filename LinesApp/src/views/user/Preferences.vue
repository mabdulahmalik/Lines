<script setup lang="ts">
import { reactive, onMounted, onUnmounted, ref, computed, watch } from 'vue';
import { FwbSelect, FwbCheckbox, FwbButton } from 'flowbite-vue';
import { useBreadcrumbStore } from '@/stores/breadcrumb';
import { useMeStore } from '@/stores/data/settings/users/me';
import { UserPreference } from '@/api/__generated__/graphql';
import { IconXClose } from '@/components/icons';

const breadcrumbStore = useBreadcrumbStore();
const meStore = useMeStore();

const me = computed(() => meStore.me);

onMounted(() => {
  breadcrumbStore.breadcrumbs = [
    { title: 'User', to: '/user/my-profile' },
    { title: 'Preferences' },
  ];
});
onUnmounted(() => {
  breadcrumbStore.breadcrumbs = [];
});

const dateNotationOptions = reactive([{ value: 'MIDDLE_ENDIAN_DATE', name: 'MM-DD-YYYY' }]);
const timeNotationOptions = reactive([{ value: 'MILITARY_TIME', name: 'Military Time (HH:mm)' }]);

const dateNotation = ref('');
const timeNotation = ref('');
const relativeTime = ref();

watch(
  () => me.value?.preferences,
  (newPreferences) => {
    if (newPreferences) {
      dateNotation.value = newPreferences.includes(UserPreference.MiddleEndianDate)
        ? UserPreference.MiddleEndianDate
        : '';
      timeNotation.value = newPreferences.includes(UserPreference.MilitaryTime)
        ? UserPreference.MilitaryTime
        : '';
      relativeTime.value = newPreferences.includes(UserPreference.ElapsedTime);
    }
  },
  { deep: true, immediate: true }
);

function handleSaveChanges() {
  const preferences = [];
  if (dateNotation.value === UserPreference.MiddleEndianDate) {
    preferences.push(dateNotation.value);
  }
  if (timeNotation.value === UserPreference.MilitaryTime) {
    preferences.push(timeNotation.value);
  }
  if (relativeTime.value) {
    preferences.push(UserPreference.ElapsedTime);
  }
  if (preferences.length) {
    meStore.modifyMyPreference({
      preferences: preferences.length ? preferences : null,
    });
  }
}
</script>
<template>
  <div class="overflow-y-auto max-h-[calc(100vh-120px)] lg:max-h-[calc(100vh-80px)] lg:p-10 p-4">
    <div class="max-w-3xl mx-auto">
      <!-- title description -->
      <div>
        <div class="font-semibold text-2xl leading-9">User Preferences</div>
        <div class="text-sm font-medium leading-[19.6px] pt-1">
          This example text is going to run a bit longer so that you can see how spacing within an
          alert works with this kind of content.
        </div>

        <div class="mt-8">
          <label class="mb-2 block text-sm font-medium text-slate-900">Date Notation</label>
          <div class="relative">
            <fwb-select
              v-model="dateNotation"
              :options="dateNotationOptions"
              placeholder="Select date notation"
            />
            <span
              v-if="dateNotation"
              @click="dateNotation = ''"
              class="absolute p-0.5 right-8 top-[50%] -mt-[9px] text-slate-500 cursor-pointer"
            >
              <IconXClose height="14" width="14" :strokeWidth="3.7" />
            </span>
          </div>
        </div>
        <div class="mt-8">
          <label class="mb-2 block text-sm font-medium text-slate-900"
            >Time Notation Preferences</label
          >
          <div class="relative">
            <fwb-select
              v-model="timeNotation"
              :options="timeNotationOptions"
              placeholder="Select time notation"
            />
            <span
              v-if="timeNotation"
              @click="timeNotation = ''"
              class="absolute p-0.5 right-8 top-[50%] -mt-[9px] text-slate-500 cursor-pointer"
            >
              <IconXClose height="14" width="14" :strokeWidth="3.7" />
            </span>
          </div>
        </div>

        <div class="mt-8 flex items-start gap-2">
          <fwb-checkbox v-model="relativeTime" class="font-medium text-sm" />
          <div>
            <div class="font-medium text-sm leading-[14px] text-slate-900">
              Display dates and times in a relative format
            </div>
            <div class="font-normal text-xs leading-[18px] text-slate-500 pt-1">
              Users can toggle an option to display dates and times in a relative format. Examples
              include "5 minutes ago," "3 hours ago," "2 days ago."
            </div>
          </div>
        </div>

        <div class="px-6 py-4 items-center flex justify-end">
          <fwb-button
            @click="handleSaveChanges"
            pill
            :disabled="!timeNotation && !dateNotation && !relativeTime"
            class="bg-primary-600 hover:bg-brand-600"
          >
            Save Changes
          </fwb-button>
        </div>
      </div>
    </div>
  </div>
</template>
