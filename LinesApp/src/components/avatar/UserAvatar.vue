<script setup lang="ts">
import { User, UserAvailability } from '@/api/__generated__/graphql';
import { FwbAvatar } from 'flowbite-vue';
import { AvatarSize } from 'flowbite-vue/components/FwbAvatar/types.js';
import { computed } from 'vue';

const props = withDefaults(
  defineProps<{
    user: User;
    rounded?: boolean;
    size?: AvatarSize;
    showStatus?: boolean;
  }>(),
  {
    size: 'sm',
    showStatus: false,
    rounded: false,
  }
);

const initials = computed(() => {
  const { firstName, lastName } = props.user;
  return firstName && lastName ? (firstName[0] + lastName[0]).toUpperCase() : '';
});

const availabilityStatus = computed(() => {
  switch (props.user.status?.status) {
    case UserAvailability.Away:
      return 'away';
    case UserAvailability.Busy:
      return 'busy';
    case UserAvailability.Free:
      return 'online';
    case UserAvailability.Offline:
      return 'offline';
    default:
      return undefined;
  }
});
</script>

<template>
  <fwb-avatar
    :img="props.user.avatar || undefined"
    :initials="initials"
    :rounded="rounded"
    :size="size"
    v-bind="showStatus ? { status: availabilityStatus } : {}"
  />
</template>