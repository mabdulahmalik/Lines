<script setup lang="ts">
import { User, UserAvailability } from '@/api/__generated__/graphql';
import { FwbAvatar } from 'flowbite-vue';
import { computed } from 'vue';

type AvatarSize = 'xs' | 'sm' | 'md' | 'lg' | 'xl';

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
    class="user_avatar"
  />
</template>

<style scoped>
.user_avatar :deep(.absolute.bg-yellow-400) {
  background-color: #E11D47;
}
.user_avatar :deep(.absolute.bg-gray-400) {
  background-color: #E3A008;
}
.user_avatar :deep(.absolute.bg-red-400) {
  background-color: #64748B;
}
</style>